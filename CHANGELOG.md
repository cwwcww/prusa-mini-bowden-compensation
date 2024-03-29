# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2024-01-14

### Added

- Compensation of print moves (with separately configured coefficients)

### Changed

- Processing of files with arc moves is not supported again

### Fixed

- Processor now crashes when bgcode is enabled instead of producing corrupted file

## [0.1.2] - 2024-01-13

### Fixed

- Improved arc travel move detection
- Fixed number formatting that occasionally led to extrusion anomalies

## [0.1.1] - 2024-01-13

### Added

- Save error details to file on crash

### Fixed

- Allow processing of files with arc print moves

## [0.1.0] - 2024-01-12

### Added

- Basic compensation for filament path changes on X travel moves.
- Calibration mode inspired by Klipper's `TUNING_TOWER`.