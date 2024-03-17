import React from 'react';
import { TodoItem } from '../types/TodoItem';
import { View, Text } from 'react-native';
import CheckBox from '@react-native-community/checkbox';

type Props = {
  todo: TodoItem;
  todos: TodoItem[];
  setTodos: (todos: TodoItem[]) => void;
};

const Todo = (props: Props): JSX.Element => {
  const toggleTodo = () => {
    const newTodos: TodoItem[] = props.todos.map(todo => {
      if (todo.id === props.todo.id) {
        todo.completed = !todo.completed;
      }
      return todo;
    });
    props.setTodos(newTodos);
  };

  return (
    <>
      <View>
        <CheckBox
          id={String(props.todo.id)}
          value={props.todo.completed}
          onValueChange={toggleTodo}
          disabled={false}
        />
        <Text>
          Todo{props.todo.id} : {props.todo.task}
        </Text>
      </View>
    </>
  );
};

export default Todo;
