import React, { useState } from 'react';
import { View } from 'react-native';
import { TodoItem } from '../types/TodoItem';
import { Button, Input } from '@rneui/base';

type Props = {
  todos: TodoItem[];
  setTodos: (todos: TodoItem[]) => void;
};

const AddTodo = (props: Props): JSX.Element => {
  const [taskText, setTaskText] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState<string>('');

  const addTodo = (): void => {
    if (taskText === null) {
      return;
    } else if (taskText === '') {
      setErrorMessage('Todoを入力してください。');
      return;
    }
    let todo: TodoItem = {
      id: 0,
      task: taskText,
      completed: false,
    };
    if (props.todos.length === 0) {
      todo.id = 1;
    } else {
      todo.id = props.todos.length + 1;
    }
    const newTodos: TodoItem[] = [...props.todos, todo];
    props.setTodos(newTodos);
    setTaskText('');
    setErrorMessage('');
  };

  return (
    <>
      <View>
        <Input
          value={taskText}
          placeholder='Todoを追加'
          errorMessage={errorMessage}
          onChangeText={newText => setTaskText(newText)}
        />
        <Button
          title='追加'
          color='primary'
          onPress={addTodo} />
      </View>
    </>
  );
};

export default AddTodo;
