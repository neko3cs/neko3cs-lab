import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoForm } from './todo-form';

describe('TodoForm', () => {
  let component: TodoForm;
  let fixture: ComponentFixture<TodoForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
