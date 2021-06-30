import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestSampleComponent } from './test-sample.component';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { BookService } from 'src/service/book/book.service';
import { Book } from 'src/data/book';

describe('TestSampleComponent', () => {
  let component: TestSampleComponent;
  let fixture: ComponentFixture<TestSampleComponent>;
  let service: BookService;
  let current: Date;
  let bookDummy: Book;

  beforeEach(async(() => {
    // Create testdouble
    bookDummy = <Book>{
      isbn: '978-4-7741-8411-1',
      title: '改訂 新版 JavaScript 本格 入門',
      price: 2980,
      publisher: '技術評論社',
    };
    let serviceStub = {
      getBooks: () => {
        return [
          bookDummy
        ];
      }
    };

    // define on testbed
    TestBed.configureTestingModule({
      declarations: [TestSampleComponent]
    })
      .overrideComponent(TestSampleComponent, {
        set: {
          providers: [
            { provide: BookService, useValue: serviceStub }
          ]
        }
      })
      .compileComponents()
      .then(result => {
        fixture = TestBed.createComponent(TestSampleComponent);
        component = fixture.componentInstance;
        service = fixture.debugElement.injector.get(BookService);
      });
  }));

  it('テーブルの行数を確認', () => {
    fixture.detectChanges();
    let des: DebugElement[] = fixture.debugElement.queryAll(By.css('#dataList tr'));
    expect(des.length).toEqual(6);
  });

  it('bookListの要素を確認', () => {
    fixture.detectChanges();
    // ISBN
    let de: DebugElement = fixture.debugElement.query(By.css('#bookList tr td:first-child'));
    expect(de.nativeElement.textContent).toEqual(bookDummy.isbn);
    // Title
    de = fixture.debugElement.query(By.css('#bookList tr td:nth-child(2)'));
    expect(de.nativeElement.textContent).toEqual(bookDummy.title);
    // Price
    de = fixture.debugElement.query(By.css('#bookList tr td:nth-child(3)'));
    expect(de.nativeElement.textContent).toEqual(bookDummy.price.toString());
    // Publisher
    de = fixture.debugElement.query(By.css('#bookList tr td:last-child'));
    expect(de.nativeElement.textContent).toEqual(bookDummy.publisher);
  })
});
