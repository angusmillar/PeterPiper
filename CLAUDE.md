# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PeterPiper is an HL7 V2.x message parsing and manipulation library for .NET, published as a NuGet package. It supports parsing, creating, modifying, and serializing HL7 v2 messages, batches, and files.

- **NuGet Package:** `PeterPiper` (version driven by Git tags — see Release Process below)
- **Target Framework:** .NET 10.0 (`net10.0`)
- **C# Language Version:** 14
- **Compiler setting:** `TreatWarningsAsErrors=true` — all warnings must be resolved

## Commands

```bash
# Build
dotnet build PeterPiper.sln
dotnet build --configuration Release

# Test (NuGet package is generated automatically on Release build)
dotnet test TestPeterPiper/TestPeterPiper.csproj
dotnet test --configuration Release

# Run a single test class
dotnet test --filter "ClassName=TestMessage"

# Run tests in a specific category
dotnet test --filter "TestCategory=Acknowledgements"
```

## Architecture

### Hierarchy

HL7 objects form a strict hierarchy reflecting the HL7 v2 structure:

```
Message → Segment → Element (repeating field) → Field → Component → SubComponent → Content
```

All levels implement `IContentBase` and `IModelBase` for a consistent API.

### Key Design Patterns

**Interface-based public API with internal implementations:**  
All public types are interfaces (`IMessage`, `ISegment`, `IElement`, `IField`, `IComponent`, `ISubComponent`, `IContent`). Concrete implementations (`Message`, `Segment`, `Field`, etc.) are internal. Consumers must always use the interfaces.

**Factory via `Creator`:**  
`Creator` (static class in `PeterPiper/Hl7/V2/Model/`) is the sole entry point for constructing any HL7 object. Never instantiate implementation classes directly.

```csharp
IMessage msg = Creator.Message(rawHl7String);
IMessage newMsg = Creator.Message("2.4", "ADT", "A01"); // template
ISegment seg = Creator.Segment("PID|...");
```

**Clone before moving:**  
Objects track their parent. To move an object from one message to another, always call `.Clone()` first.

**One-based indexing:**  
Segments, fields, elements, components, and subcomponents all use 1-based indexing (consistent with HL7 spec). The only exception is `Content` items within a subcomponent.

### HL7-Specific Concepts

**Escape handling:**  
- `.AsString` — auto-unescapes HL7 escape sequences when reading, auto-escapes when writing  
- `.AsStringRaw` / `.AsRawString` — raw access bypassing escape processing  

**HL7 null vs empty:**  
- `.IsEmpty` — field is absent (`||`)  
- `.IsHL7Null` — field is explicitly null (`|""|`) per the HL7 spec delete semantic  

**Container types:**  
- `IMessage` — a single HL7 message (starts with MSH)  
- `IBatch` — multiple messages wrapped with BHS/BTS  
- `IFile` — multiple batches wrapped with FHS/FTS  

**Custom delimiters:**  
Non-standard encoding characters are supported via `IMessageDelimiters`, which can be passed when constructing segments or messages.

### Project Layout

```
PeterPiper/                   # Library project
  Hl7/V2/
    Model/                    # Interfaces + Creator factory + implementations
    Support/
      Content/Convert/        # DateTime, Integer, Base64 converters
      Standard/               # Delimiters, escapes, HL7 tables, ACK helpers
      TextFile/               # Stream-based HL7 readers/writers
    Schema/                   # XSD-based message structure validation
    CustomException/          # PeterPiperException (only exception type thrown)

TestPeterPiper/               # MSTest test project
  TestModel/                  # Tests per model type (TestMessage, TestSegment, etc.)
  TestSchema/                 # XSD schema validation tests
  TestResource/               # Raw HL7 test data files
```

### Exception Handling

The library throws only `PeterPiperException` (in `PeterPiper.Hl7.V2.CustomException`). Catch this type for all library errors.

### Tests

Uses **MSTest** (`Microsoft.VisualStudio.TestTools.UnitTesting`). Tests are organized by model type and feature area. Test data (raw HL7 strings) lives in `TestPeterPiper/TestResource/`.

## Release Process

Releases are fully automated via GitHub Actions (`.github/workflows/publish-nuget.yml`). Pushing a tag triggers the workflow: tests run, the NuGet package is packed with the version extracted from the tag, published to NuGet.org, and a GitHub Release is created.

**Tag format:** `V<major>.<minor>.<patch>` (e.g. `V10.0.0`) — the leading `V` is stripped to produce the NuGet package version (`10.0.0`).

```bash
# To publish a new release from the Release branch:
git tag V10.0.0
git push origin V10.0.0
```

**One-time setup required:** A `NUGET_API_KEY` secret must be set in the GitHub repository (Settings → Secrets and variables → Actions). Generate the key at nuget.org scoped to the `PeterPiper` package.
