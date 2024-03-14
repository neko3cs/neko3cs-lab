import { Component, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { UserListFilter } from '../state';

@Component({
  selector: 'user-list-filter',
  standalone: true,
  imports: [
    ReactiveFormsModule,
  ],
  templateUrl: './user-list-filter.component.html',
  styleUrls: ['./user-list-filter.component.css']
})
export class UserListFilterComponent implements OnDestroy {
  @Input() set value(value: UserListFilter | null) {
    if (value != null) {
      this.setFormValue(value);
    }
  }
  @Output() valueChange = new EventEmitter<UserListFilter>();

  form: FormGroup;

  private onDestroy = new EventEmitter();

  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      nameFilter: ['']
    });
    this.form.valueChanges.pipe(takeUntil(this.onDestroy)).subscribe(value => {
      this.valueChange.emit(value);
    });
  }

  ngOnDestroy() {
    this.onDestroy.complete();
  }

  private setFormValue(value: UserListFilter) {
    this.form.setValue(value);
  }
}
