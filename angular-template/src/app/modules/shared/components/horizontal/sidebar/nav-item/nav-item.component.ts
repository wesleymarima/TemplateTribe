import {Component, Input, OnInit,} from '@angular/core';
import {Router} from '@angular/router';
import {NavService} from '../../../../services/nav.service';

@Component({
  selector: 'app-horizontal-nav-item',
  standalone: false,
  templateUrl: './nav-item.component.html',
})
export class AppHorizontalNavItemComponent implements OnInit {
  @Input() depth: any;
  @Input() item: any;

  constructor(public navService: NavService, public router: Router) {
    if (this.depth === undefined) {
      this.depth = 0;
    }
  }

  ngOnInit() {
  }

  onItemSelected(item: any) {
    if (!item.children || !item.children.length) {
      this.router.navigate([item.route]);
    }
  }
}
