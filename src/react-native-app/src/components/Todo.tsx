import React from 'react';
import { TodoItem } from '../types/TodoItem';
import { View } from 'react-native';
import { CheckBox } from '@rneui/base';
import { globalStyles } from '../GlobalStyles';

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
      <View
        style={[globalStyles.items_large, globalStyles.container]}>
        <CheckBox
          id={String(props.todo.id)}
          checked={props.todo.completed}
          title={`No.${props.todo.id} : ${props.todo.task}`}
          style={globalStyles.items_large}
          checkedColor='#0F0'
          uncheckedColor='#F00'
          size={30}
          containerStyle={{ width: '75%' }}
          onIconPress={toggleTodo}
        />
      </View>
    </>
  );
};

export default Todo;
