# DataEncoding Library

A .NET Standard 2.0 library for serializing and deserializing data using DER, PEM, JSON and XML formats.

## Usage

### Library organization

All of this library's classes are under the `DataEncoding` namespace.
This namespace contains other namespaces representing the various formats supported by this library.
For example all classes for working with JSON format are located in the `DataEncoding.JSON` namespace.

### Serialization

#### Manual

All serializable classes in the namespaces that represent various serialization formats provide `Encode()` method, which serializes the contents of an object into the given format.
Below is an example of serializing a JSON object, represented by `JSONObject` class in `DataEncoding.JSON` namespace.

```cs
JSONObject obj = new JSONObject();
obj.Content.Add("a", new JSONNumber(5));
obj.Content.Add("b", new JSONString("hello"));

obj.Encode();   // Result: {"a":5,"b":"hello"}
```

#### Reflection

This library also provides serialization of objects based on reflection, through classes in the `DataEncoding.Reflection` namespace.
To serialize an object into a format supported by this library, use an appropriate serializer from the `DataEncoding.Reflection.Serializers` namespace.
For example, to serialize an object into JSON, use `SerializerJSON` like this:

```cs
Tuple<string, int> obj = new Tuple<string, int>("a", 5);
SerializerJSON.Serialize(obj);      // Result: {"Item1":"a","Item2":"5"}
```

### Deserialization

#### Manual

To deserialize objects, use a `Decode(input)` method on an appropriate object, based on what you want to deserialize and from what format.
The deserialized data replaces content of the object the method was called on.

```cs
JSONObject obj = new JSONObject();
obj.Decode("{'a':'hello','b':false}");

JSONBase a = obj.Content.Find("a");
if (a is JSONString s)
    s.Content;                      // Result: "hello"

obj.Content.Find("c");              // Result: null
```

#### Reflection

**Not yet implemented.**

### Serialization attributes

Behavior of reflection-based serializers can be affected by C# annotations.
Use `SerializableStructureAttribute` on class, struct or interface, to specify class-level preferences and `SerializablePropertyAttribute` on field or property to specify property-level preferences.

```cs
[SerializableStructure(ForcedCase = ForcedCase.Lower)]
class A
{
    public int A => 1;

    [SerializableProperty(Include = false)]
    public int B => 2;

    [SerializableProperty(Include = true, Name = "F")]
    private int C => 3;
}

SerializerJSON.Serialize(new A());  // Result: {"A":1,"F":3}
```

## License

This library is licensed under the **GNU Lesser General Public License v3**.

## Building the library

### Visual Studio

Open either the *.csproj* or *.sln* file in the root of this repository in Visual Studio and build the project.
Alternatively you can download a build from the [releases section of the GitHub repository](https://github.com/VojtechKursa/DataEncoding/releases).

### .NET CLI

Run the following command in the root of the repository:

```sh
dotnet build
```

## Contributing

This library is licensed under the **GNU Lesser General Public License v3** so anyone can use it for free and modify it as long as that person follows the conditions stated by the license.
If you have suggestions that would optimize or otherwise improve this library feel free to share them by creating an issue or submitting a pull request with your implemented ideas in the GitHub repository (see section *Source code & Repository*).

## Source code & Repository

The source code for this library and it's public repository can be found on [GitHub](https://github.com/VojtechKursa/DataEncoding).
