import {Component, OnInit} from '@angular/core';
import {Subject} from 'rxjs';
import {LoaderService} from '../../services/loader.service';

@Component({
  selector: 'app-shared-loading-spinner',
  standalone: false,
  templateUrl: './shared-loading-spinner.component.html',
  styleUrl: './shared-loading-spinner.component.scss'
})
export class SharedLoadingSpinnerComponent implements OnInit {
  isLoading!: Subject<boolean>;

  constructor(private loaderService: LoaderService) {
  }

  ngOnInit(): void {
    this.isLoading = this.loaderService.isLoading;
  }
}
