import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BaseSharedService {

  dashboardTitle = signal('Template Tribe');

  updateTitle(newTitle: string): void {
    this.dashboardTitle.set(newTitle);
  }
}
