# Tablewright

Tablewright is a work-in-progress .NET library for describing, validating, and importing tabular data with schema-first semantics.

The project is currently very early-stage and not ready for production use. The immediate goal is to build a small, well-structured core model for tabular schemas, validation diagnostics, logical column types, and eventually CSV import.

## Status and Roadmap

This project is not complete.

At the moment, the focus is on the core schema and validation model. CSV parsing, CSVW/Frictionless compatibility, CLI tooling, schema inference, and high-performance typed materialisation are intended future areas, but they should not be assumed to exist yet.

### Intended direction

The intended library shape is roughly:

schema definition → schema validation → source binding → data validation → typed tabular access

The project is especially interested in awkward real-world import problems: inconsistent headers, missing values, null tokens such as N/A, type conversion failures, duplicate keys, and diagnostics that explain what went wrong without immediately throwing exceptions.
