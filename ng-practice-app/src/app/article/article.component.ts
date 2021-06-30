import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css']
})
export class ArticleComponent implements OnInit {
  id: '';
  nextId: '';

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route
      .params
      .subscribe(
        params => this.id = params['id']
      );
  }

  onclick() {
    if (!this.nextId) {
      alert('No input error!');
      return;
    }
    this.router.navigate(['/article', this.nextId]);
  }

}
