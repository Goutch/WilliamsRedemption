@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

SET DIR=%~dp0

IF NOT "!DIR!"=="!DIR: =!" (
    ECHO == Code Generation failed ==
    ECHO The project path has spaces in it. Please move this project to a path without spaces in it.
    PAUSE
    EXIT /B 1
)

CALL Assets\Libraries\Harmony\Scripts\Editor\Tools\CodeGenerator\CodeGenerator.bat "!DIR!" "!DIR!Assets\Generated"

IF %ERRORLEVEL% EQU 0 (
    ECHO == Code Generation succeeded ==
    PAUSE
    EXIT /B 0
) ElSE (
    ECHO == Code Generation failed ==
    PAUSE
    EXIT /B 1
)