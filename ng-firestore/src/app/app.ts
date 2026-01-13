import { Component, inject, signal } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { Firestore } from '@angular/fire/firestore';
import { Person } from './person';

@Component({
  selector: 'app-root',
  imports: [MatCard, MatButtonModule, MatInputModule, MatIconModule, FormsModule, MatTableModule],
  styles: [],
  template: `
    <mat-card style="max-width: 400px; margin: 2rem auto; padding: 1rem;">
      <h1>Angular Firestore Sample</h1>

      <mat-form-field appearance="outline" style="width: 100%; margin-top: 1rem;">
        <input matInput [value]="name()" placeholder="名前を入力" (input)="name.set($event.target.value)" />
      </mat-form-field>
      <mat-form-field appearance="outline" style="width: 100%; margin-top: 1rem;">
        <input matInput [value]="age()" type="number" placeholder="年齢を入力"
                (input)="age.set($any($event.target).valueAsNumber)" />
      </mat-form-field>

      <div style="text-align: center;">
        <button mat-raised-button color="accent" (click)="add()" style="width: 100%;">
          登録
        </button>
      </div>

      <table mat-table [dataSource]="people()" class="mat-elevation-z8" style="width: 100%; margin-top: 1rem;">
        <ng-container matColumnDef="Name">
          <th mat-header-cell *matHeaderCellDef> 名前 </th>
          <td mat-cell *matCellDef="let p"> {{ p.Name }} </td>
        </ng-container>
        <ng-container matColumnDef="Age">
          <th mat-header-cell *matHeaderCellDef> 年齢 </th>
          <td mat-cell *matCellDef="let p"> {{ p.Age }} </td>
        </ng-container>
        <ng-container matColumnDef="Remove">
          <th mat-header-cell *matHeaderCellDef> 削除 </th>
          <td mat-cell *matCellDef="let p">
            <button mat-icon-button color="warn" (click)="remove(p.Id)">
              <mat-icon>delete</mat-icon>
            </button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="['Name', 'Age', 'Remove']"></tr>
        <tr mat-row *matRowDef="let row; columns: ['Name', 'Age', 'Remove']"></tr>
      </table>
    </mat-card>
  `,
})
export class App {
  private firestore = inject(Firestore);

  name = signal('');
  age = signal(0);
  people = signal<Person[]>([]);

  add(): void {
    const person: Person = { Id: crypto.randomUUID(), Name: this.name(), Age: this.age() };
    this.people.update(list => [...list, person]);
    this.name.set('');
    this.age.set(0);
  }

  remove(id: string): void {
    this.people.update(list => list.filter(p => p.Id !== id));
  }
}
