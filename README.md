kv (Key-Value storage)
> Trivial Key-Value store (for low-volume storage, terminal-centric)

## WIP: Work In Progress (not functional)

Q: Does unix have a standard solution?\
A: Not really. https://unix.stackexchange.com/questions/21943/standard-key-value-datastore-for-unix

Q: What is the closest thing?\
A: GNU has a project called [GDBM](https://www.gnu.org.ua/software/gdbm/), which is under active development

Alternative approached:

- [SQLite](https://www.sqlite.org/index.html)

## Key Ideas

- Terminal/CLI centric
- Implicit security by default (unix file permissions)
- No transactions
- Fat Locks (slow, but easy to implement)
- Functions (allow mapping to other resources, or stored-procedure like scripting)
    - Setup forwarding to cloud KV stores
- Simple .json storage, 1 file per datasource/namespace
- Simple encryption per file
- Limited meta-data (last/first read/write)
- Abstracted back-end (same interface, multiple storage mechanisms: file-json, file-toml, file-csv, sqlite, sql, REST)

## CLI (Command Line Interface)

Commands: { get, set, ls, rm } Flags: --stdin

Examples

```bash

# Init:
# Create a new empty Store called 'Demo'
$ kv init Demo

$ kv set Demo:Hello <<< "World!"

$ kv get -ds Demo -key Hello
World!

$ kv get Demo:Hello
World!

$ kv ls Demo
Hello

$ kv rm Demo Hello

$ export KVdefault=Demo

$ kv get Hello
World!
```
