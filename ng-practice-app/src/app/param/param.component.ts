import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-param',
  templateUrl: './param.component.html',
  styleUrls: ['./param.component.css']
})
export class ParamComponent implements OnInit {
  query: string = '';
  fragment: string = '';

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(
        params => this.query = `${params['category']}/${params['keyword']}`
      );
    this.route
      .fragment
      .subscribe(
        frag => this.fragment = frag
      );
  }

}
