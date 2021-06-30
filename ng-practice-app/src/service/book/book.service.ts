import { Injectable } from '@angular/core';
import { Book } from '../../data/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  public getBooks(): Book[] {
    return [
      {
        isbn: '978-4-7741-8411-1',
        title: '改訂 新版 JavaScript 本格 入門',
        price: 2980,
        publisher: '技術評論社',
      },
      {
        isbn: '978-4-7980-4853-6',
        title: 'はじめて の Android アプリ 開発 第 2 版',
        price: 3200,
        publisher: '秀和 システム',
      },
      {
        isbn: '978-4-7741-8030-4',
        title: '［改訂 新版］ Java ポケット リファレンス',
        price: 2680,
        publisher: '技術評論社',
      },
      {
        isbn: '978-4-7981-3547-2',
        title: '独習 PHP 第 3 版',
        price: 3200,
        publisher: '翔 泳 社',
      },
      {
        isbn: '978-4-8222-9893-7',
        title: '基礎 から しっかり 学ぶ C ++ の 教科書',
        price: 2800,
        publisher: '日経 BP 社',
      }
    ];
  }
}
