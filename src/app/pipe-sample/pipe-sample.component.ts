import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-pipe-sample',
  templateUrl: './pipe-sample.component.html',
  styleUrls: ['./pipe-sample.component.css']
})
export class PipeSampleComponent implements OnInit {
  memo: string = '';

  constructor() { }

  ngOnInit() {
  }

}
