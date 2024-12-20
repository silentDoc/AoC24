
## --- Day 20: Race Condition ---

The Historians are quite pixelated again. This time, a massive, black building looms over you - you're  [right outside](https://adventofcode.com/2017/day/24)  the CPU!

While The Historians get to work, a nearby program sees that you're idle and challenges you to a  _race_. Apparently, you've arrived just in time for the frequently-held  _race condition_  festival!

The race takes place on a particularly long and twisting code path; programs compete to see who can finish in the  _fewest picoseconds_. The  winner  even gets their very own  [mutex](https://en.wikipedia.org/wiki/Lock_(computer_science))!

They hand you a  _map of the racetrack_  (your puzzle input). For example:

```
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############

```

The map consists of track (`.`) - including the  _start_  (`S`) and  _end_  (`E`) positions (both of which also count as track) - and  _walls_  (`#`).

When a program runs through the racetrack, it starts at the start position. Then, it is allowed to move up, down, left, or right; each such move takes  _1 picosecond_. The goal is to reach the end position as quickly as possible. In this example racetrack, the fastest time is  `84`  picoseconds.

Because there is only a single path from the start to the end and the programs all go the same speed, the races used to be pretty boring. To make things more interesting, they introduced a new rule to the races: programs are allowed to  _cheat_.

The rules for cheating are very strict.  _Exactly once_  during a race, a program may  _disable collision_  for up to  _2 picoseconds_. This allows the program to  _pass through walls_  as if they were regular track. At the end of the cheat, the program must be back on normal track again; otherwise, it will receive a  [segmentation fault](https://en.wikipedia.org/wiki/Segmentation_fault)  and get disqualified.

So, a program could complete the course in 72 picoseconds (saving  _12 picoseconds_) by cheating for the two moves marked  `1`  and  `2`:

```
###############
#...#...12....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############

```

Or, a program could complete the course in 64 picoseconds (saving  _20 picoseconds_) by cheating for the two moves marked  `1`  and  `2`:

```
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...12..#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############

```

This cheat saves  _38 picoseconds_:

```
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.####1##.###
#...###.2.#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############

```

This cheat saves  _64 picoseconds_  and takes the program directly to the end:

```
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..21...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############

```

Each cheat has a distinct  _start position_  (the position where the cheat is activated, just before the first move that is allowed to go through walls) and  _end position_; cheats are uniquely identified by their start position and end position.

In this example, the total number of cheats (grouped by the amount of time they save) are as follows:

-   There are 14 cheats that save 2 picoseconds.
-   There are 14 cheats that save 4 picoseconds.
-   There are 2 cheats that save 6 picoseconds.
-   There are 4 cheats that save 8 picoseconds.
-   There are 2 cheats that save 10 picoseconds.
-   There are 3 cheats that save 12 picoseconds.
-   There is one cheat that saves 20 picoseconds.
-   There is one cheat that saves 36 picoseconds.
-   There is one cheat that saves 38 picoseconds.
-   There is one cheat that saves 40 picoseconds.
-   There is one cheat that saves 64 picoseconds.

You aren't sure what the conditions of the racetrack will be like, so to give yourself as many options as possible, you'll need a list of the best cheats.  _How many cheats would save you at least 100 picoseconds?_