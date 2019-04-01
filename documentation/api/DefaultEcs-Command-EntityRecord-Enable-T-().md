#### [DefaultEcs](./DefaultEcs.md 'DefaultEcs')
### [DefaultEcs.Command](./DefaultEcs.md#DefaultEcs-Command 'DefaultEcs.Command').[EntityRecord](./DefaultEcs-Command-EntityRecord.md 'DefaultEcs.Command.EntityRecord')
## Enable&lt;T&gt;() `method`
Enables the corresponding [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') component of type [T](#DefaultEcs-Command-EntityRecord-Enable-T-()-T 'DefaultEcs.Command.EntityRecord.Enable&lt;T&gt;().T') so it can appear in [EntitySet](./DefaultEcs-EntitySet.md 'DefaultEcs.EntitySet').
Does nothing if corresponding [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') does not have a component of type [T](#DefaultEcs-Command-EntityRecord-Enable-T-()-T 'DefaultEcs.Command.EntityRecord.Enable&lt;T&gt;().T').
This command takes 9 bytes.
### Type parameters

<a name='DefaultEcs-Command-EntityRecord-Enable-T-()-T'></a>
`T`

The type of the component.