using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.TodoLists.Commands.CreateTodoList;
using TemplateAPI.Application.TodoLists.Commands.UpdateTodoList;
using TemplateAPI.Domain.Entities;
using NotFoundException = TemplateAPI.Application.Common.Exceptions.NotFoundException;

namespace TemplateAPI.Application.FunctionalTests.TodoLists.Commands;

using static Testing;

public class UpdateTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        UpdateTodoListCommand command = new() { Id = 99, Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        int listId = await SendAsync(new CreateTodoListCommand { Title = "New List" });

        await SendAsync(new CreateTodoListCommand { Title = "Other List" });

        UpdateTodoListCommand command = new() { Id = listId, Title = "Other List" };

        (await FluentActions.Invoking(() =>
                    SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
            .And.Errors["Title"].Should().Contain("'Title' must be unique.");
    }

    [Test]
    public async Task ShouldUpdateTodoList()
    {
        string userId = await RunAsDefaultUserAsync();

        int listId = await SendAsync(new CreateTodoListCommand { Title = "New List" });

        UpdateTodoListCommand command = new() { Id = listId, Title = "Updated List Title" };

        await SendAsync(command);

        TodoList? list = await FindAsync<TodoList>(listId);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.LastModifiedBy.Should().NotBeNull();
        list.LastModifiedBy.Should().Be(userId);
        list.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
