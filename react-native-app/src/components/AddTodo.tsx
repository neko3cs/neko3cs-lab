import React, { useState } from 'react';
import { Alert, Button, TextInput, View } from 'react-native';
import { TodoItem } from '../types/TodoItem';

type Props = {
  todos: TodoItem[];
  setTodos: (todos: TodoItem[]) => void;
};

const AddTodo = (props: Props): JSX.Element => {
  const [taskText, setTaskText] = useState<string>('');

  const addTodo = (): void => {
    if (taskText === null) {
      return;
    } else if (taskText === '') {
      return Alert.alert('Todoを入力してください。');
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
  };

  return (
    <>
      <View>
        <TextInput
          placeholder='Todoを追加'
          value={taskText}
          onChangeText={newText => setTaskText(newText)}
        />
        <Button title='追加' onPress={addTodo} />
      </View>
    </>
  );
};

export default AddTodo;
