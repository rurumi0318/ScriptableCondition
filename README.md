# About this project
`ScriptableCondition` allows you to verify a public field of a scriptable object with certain comparison methods.
![Example image](https://github.com/rurumi0318/ScriptableCondition/blob/master/example.png?raw=true)

## Usage
Let's say you have a class `TestData` inherited from `ScriptableObject` with a public int field: `intField`
And you want to verify an instance of `TestData` to see if it's `intField` is greater or equal to 100.

1. Attach the `ScriptableCondition` to a game object
2. Reference the `Data template` field to any instance of `TestData`
3. Set `Field to verify` to `intField`
4. Set `Operator` to `>=`
5. Set `Compare to` to `100`
6. In any other scripts, call `scriptableCondition.Verify(instance_to_verify)`
7. It returns `true` if the `instance_to_verify` has the `intField` and the value is greater or equal to `100`