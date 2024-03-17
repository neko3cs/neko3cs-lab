import React from 'react';
import { TodoItem } from '../types/TodoItem';
import { Button } from 'react-native';

type Props = {
  todos: TodoItem[];
  setTodos: (todos: TodoItem[]) => void;
};

const DeleteTodo = (props: Props): JSX.Element => {
  const deleteTodo = () => {
    const newTodos: TodoItem[] = props.todos.filter(todo => !todo.completed);
    let idCount = 1;
    newTodos.forEach(todo => {
      todo.id = idCount;
      idCount++;
    });
    props.setTodos(newTodos);
  };

  return (
    <>
      <Button title='完了したTodoを削除' onPress={deleteTodo} />
    </>
  );
};

export default DeleteTodo;
