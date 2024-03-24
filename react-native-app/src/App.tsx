import React, { useState } from 'react';
import {
  ScrollView,
  View,
} from 'react-native';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { Header, Text } from '@rneui/base';
import { TodoItem } from './types/TodoItem';
import AddTodo from './components/AddTodo';
import TodoList from './components/TodoList';
import DeleteTodo from './components/DeleteTodo';
import { globalStyles } from './GlobalStyles';

const App = (): React.JSX.Element => {
  const [todos, setTodos] = useState<TodoItem[]>([]);

  return (
    <SafeAreaProvider>
      <Header
        centerComponent={{
          text: 'TODO APP',
          style: { color: '#fff' },
        }} />
      <ScrollView
        contentInsetAdjustmentBehavior="automatic">
        <View style={globalStyles.container}>
          <AddTodo todos={todos} setTodos={setTodos} />
          <TodoList todos={todos} setTodos={setTodos} />
          <Text h4>残りTodo件数 : {todos.length}件</Text>
          <DeleteTodo todos={todos} setTodos={setTodos} />
        </View>
      </ScrollView>
    </SafeAreaProvider>
  );
};

export default App;
