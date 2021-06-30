import { Component, OnInit } from '@angular/core';
import { Book } from '../../data/book';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getAll();
  }

  getAll() {
    this.http.get<Book[]>(
      '/api/books'
    ).subscribe(
      response => {
        console.log(response);
        this.books = response;
      }
    );
  }


}
