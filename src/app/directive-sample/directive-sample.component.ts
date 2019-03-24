import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-directive-sample',
  templateUrl: './directive-sample.component.html',
  styleUrls: ['./directive-sample.component.css']
})
export class DirectiveSampleComponent implements OnInit {
  end: Date;

  constructor() { }

  ngOnInit() {
    this.end = new Date(2019, 4, 1);
  }

}
