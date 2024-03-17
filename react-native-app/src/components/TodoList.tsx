import React from 'react';
import { View } from 'react-native';
import { TodoItem } from '../types/TodoItem';
import Todo from './Todo';

type Props = {
  todos: TodoItem[];
  setTodos: (todos: TodoItem[]) => void;
};

const TodoList = (props: Props): JSX.Element => {
  return (
    <>
      {props.todos.map(todo => {
        return (
          <View key={todo.id}>
            <Todo todo={todo} todos={props.todos} setTodos={props.setTodos} />
          </View>
        );
      })}
    </>
  );
};

export default TodoList;
