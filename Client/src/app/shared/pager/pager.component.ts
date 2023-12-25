import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss'],
})
export class PagerComponent {
  @Input('totalCount') totalCount = 0;
  @Input('pageSize') pageSize = 6;
  @Output('pageChanged') selectedPageNumber = new EventEmitter<number>();

  onPagerChanged(event: any) {
    this.selectedPageNumber.emit(event.page);
  }
}
