import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss'],
})
export class PagingHeaderComponent {
  @Input('pageNumber') pageNumber = 0;
  @Input('pageSize') pageSize = 6;
  @Input('totalCount') totalCount = 0;
}
