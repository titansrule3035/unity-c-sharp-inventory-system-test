# CSharpToDoList

A simple console-based To-Do List application written in C#.
Tasks are stored in an **encoded format** for basic obfuscation, making the saved file unreadable in plain text.

## Features

* Add new tasks
* View all tasks
* Mark tasks as completed
* Remove tasks
* Tasks are **saved to disk in an encoded format**

## How It Works

Each task is stored as:

```
<description>|<completed_flag>
```

* `description` --> the task text
* `completed_flag` --> `1` if completed, `0` if not

Before saving, the task string is encoded using a **two-layer encoding system**:

1. Convert each character to an 8-bit binary string.
2. Base64-encode the resulting binary string.
3. Convert the Base64 string into binary again for final storage.

When loading, the program reverses these steps using its decoding function, restoring the original task text.

### Example

A task like:

```
Buy groceries|0
```

might appear in `myTasks.txt` as a series of binary numbers like:

```
01000010 01110101 01111001 00100000 01100111 01110010 ...
```

The program automatically decodes it back to `Buy groceries|0` when reading the file.

## Getting Started

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) installed

### Running the Application

```bash
dotnet run
```

## Usage

Follow the on-screen menu to add, view, complete, or remove tasks. Use the number keys to choose an option and enter/return to confirm.

### Example Menu

```
1. View Tasks
2. Add Task
3. Mark Task Completed
4. Delete Task
5. Clear All Tasks
6. Save & Exit
```

## License

This project is licensed under the MIT License in its parent repository.
