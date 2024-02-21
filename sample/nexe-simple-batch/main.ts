import { Command } from "commander";

const main = (name: string) => {
  console.log(`Hello, ${name}`);
}

const command = new Command();
command
  .name("HelloCommand")
  .version("1.0.0")
  .description("Say hello command.")
  .option("--name <your_name>")
  .parse();
const options = command.opts();

main(options.name);
