# Features

# Infrastructure
- `[]` Validation layers
  - `[]` Command-level validation system
    - A consistent framework for validation requests before command logic gets executed
    - Static validation - validation that only needs to consider the request itself
      - Simple validations like field length, nullability, etc
    - Dynamic validation - validation that needs to consider the state of the system
      - I.e. when adding a resume to a website we must check if that resume exists
      - ```csharp
        public void Validate(I input) {
          new Validator(input)
            .Check("fullName", i => i.FullName,
        }
        ```

- `[]` Trim strings when deserializing

# Bugs
