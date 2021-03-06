#### [DefaultEcs](./index.md 'index')
### [DefaultEcs](./DefaultEcs.md 'DefaultEcs').[EntitySetBuilder](./DefaultEcs-EntitySetBuilder.md 'DefaultEcs.EntitySetBuilder').[EitherBuilder](./DefaultEcs-EntitySetBuilder-EitherBuilder.md 'DefaultEcs.EntitySetBuilder.EitherBuilder')
## EitherBuilder.WhenRemovedEither&lt;T&gt;() Method
Makes a rule to observe [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') when one component of the either group is removed.  
```C#
public DefaultEcs.EntitySetBuilder.EitherBuilder WhenRemovedEither<T>();
```
#### Type parameters
<a name='DefaultEcs-EntitySetBuilder-EitherBuilder-WhenRemovedEither-T-()-T'></a>
`T`  
The type of component to add to the either group.  
  
#### Returns
[EitherBuilder](./DefaultEcs-EntitySetBuilder-EitherBuilder.md 'DefaultEcs.EntitySetBuilder.EitherBuilder')  
A [EitherBuilder](./DefaultEcs-EntitySetBuilder-EitherBuilder.md 'DefaultEcs.EntitySetBuilder.EitherBuilder') to create a either group.  
