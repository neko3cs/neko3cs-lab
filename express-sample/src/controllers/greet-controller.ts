import { Controller, Get, Param } from "routing-controllers";

@Controller('/greeting')
export class GreetController {

  @Get('/hello-world')
  helloWorld() {
    return 'hello world!';
  }

  @Get('/hello-world/:name')
  helloPerson(@Param('name') name: string) {
    return `hello ${name}!`;
  }

}
