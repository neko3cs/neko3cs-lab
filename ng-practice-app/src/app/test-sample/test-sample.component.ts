import { Component, OnInit } from '@angular/core';
import { BookService } from 'src/service/book/book.service';
import { Book } from 'src/data/book';

@Component({
  selector: 'app-test-sample',
  templateUrl: './test-sample.component.html',
  styleUrls: ['./test-sample.component.css']
})
export class TestSampleComponent implements OnInit {
  data = [
    { id: 1, data: 'A' },
    { id: 2, data: 'B' },
    { id: 3, data: 'C' },
    { id: 4, data: 'D' },
    { id: 5, data: 'E' }
  ];
  books: Book[];

  constructor(private book: BookService) { }

  ngOnInit() {
    this.books = this.book.getBooks();
  }

}
