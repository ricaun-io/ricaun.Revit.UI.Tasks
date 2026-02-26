# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.1] / 2026-02-26
### Features
- Build for multiple target frameworks: `net46`, `net47`, `net48`, `net8.0-windows`, `net10.0-windows`.
### Updates
- Update `nupkg` configuration with license and tags. (Fix: #1)
- Update `Build` configuration to `net10`.

## [1.0.0] / 2024-01-27 - 2024-02-20
### Features
- Support `RevitTaskService` with `IRevitTask`.
- Support `CancellationToken` in `IRevitTask`.
- Support execute code `InContext` instead of wait.
### Added
- Add `RevitTaskService`
- Add `IRevitTask`
- Add `IRevitTaskExtension`
- Add `RevitInContextExtension` and `UIControlledApplicationExtension`

[vNext]: ../../compare/1.0.0...HEAD
[1.0.1]: ../../compare/1.0.0...1.0.1
[1.0.0]: ../../compare/1.0.0