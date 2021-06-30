import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HatenaService } from 'src/service/hatena/hatena.service';

@Component({
  selector: 'app-hatena',
  templateUrl: './hatena.component.html',
  styleUrls: ['./hatena.component.css']
})
export class HatenaComponent implements OnInit {
  url = 'http://gihyo.jp/'
  comments: string[] = [];

  constructor(private route: ActivatedRoute, private hatena: HatenaService) { }

  ngOnInit() {
    let result: string[] = [];
    this.route
      .data
      .subscribe(
        data => {
          for (let value of data['hatena'].bookmarks) {
            if (value.comment !== '') {
              result.push(value.comment);
            }
          }
          this.comments = result;
        }
      );
  }

  onclick() {
    let result: string[] = [];
    this.hatena
      .requestGet(this.url)
      .subscribe(
        data => {
          for (let value of data.bookmarks) {
            if (value.comment !== '') {
              result.push(value.comment);
            }
          }
          this.comments = result;
        }
      );
  }

}
