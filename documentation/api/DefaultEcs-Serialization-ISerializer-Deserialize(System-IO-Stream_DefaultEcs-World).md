#### [DefaultEcs](./index.md 'index')
### [DefaultEcs.Serialization](./DefaultEcs-Serialization.md 'DefaultEcs.Serialization').[ISerializer](./DefaultEcs-Serialization-ISerializer.md 'DefaultEcs.Serialization.ISerializer')
## ISerializer.Deserialize(System.IO.Stream, DefaultEcs.World) Method
Deserializes [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') instances with their components from the given [System.IO.Stream](https://docs.microsoft.com/en-us/dotnet/api/System.IO.Stream 'System.IO.Stream') into the given [World](./DefaultEcs-World.md 'DefaultEcs.World').  
```C#
System.Collections.Generic.ICollection<DefaultEcs.Entity> Deserialize(System.IO.Stream stream, DefaultEcs.World world);
```
#### Parameters
<a name='DefaultEcs-Serialization-ISerializer-Deserialize(System-IO-Stream_DefaultEcs-World)-stream'></a>
`stream` [System.IO.Stream](https://docs.microsoft.com/en-us/dotnet/api/System.IO.Stream 'System.IO.Stream')  
The [System.IO.Stream](https://docs.microsoft.com/en-us/dotnet/api/System.IO.Stream 'System.IO.Stream') from which the data will be loaded.  
  
<a name='DefaultEcs-Serialization-ISerializer-Deserialize(System-IO-Stream_DefaultEcs-World)-world'></a>
`world` [World](./DefaultEcs-World.md 'DefaultEcs.World')  
The [World](./DefaultEcs-World.md 'DefaultEcs.World') instance on which the [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') will be created.  
  
#### Returns
[System.Collections.Generic.ICollection&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.ICollection-1 'System.Collections.Generic.ICollection`1')[Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.ICollection-1 'System.Collections.Generic.ICollection`1')  
The [Entity](./DefaultEcs-Entity.md 'DefaultEcs.Entity') instances loaded.  
