const Command = require("commander");

// const command = new Command();
Command.option("--name <char>");

Command.parse();

const yourName = Command.opts().name;
console.log(`Hello, ${yourName}`);
