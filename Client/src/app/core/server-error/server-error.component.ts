import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss'],
})
export class ServerErrorComponent {
  error?: any;
  constructor(private router: Router) {
    const navigation = router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }
}
