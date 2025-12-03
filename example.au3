#include <GUIConstantsEx.au3>
#include <MsgBoxConstants.au3>

; 1. Create the Graphical User Interface (GUI)
Local $hGUI = GUICreate("My First AutoIt Script", 300, 150)

; Add an input box and a button
GUICtrlCreateLabel("Enter text to type into Notepad:", 10, 10)
Local $idInput = GUICtrlCreateInput("Hello World!", 10, 30, 280, 20)
Local $idBtnGo  = GUICtrlCreateButton("Launch & Type", 50, 70, 200, 40)

; Show the GUI
GUISetState(@SW_SHOW)

; 2. The Main Loop (Keeps the script running)
While 1
    ; Listen for user actions
    Switch GUIGetMsg()
        Case $GUI_EVENT_CLOSE
            ; User clicked the 'X' to close
            ExitLoop

        Case $idBtnGo
            ; User clicked the button
            RunAutomation()
    EndSwitch
WEnd

; 3. The Function that does the work
Func RunAutomation()
    ; Read what the user typed in the box
    Local $sMessage = GUICtrlRead($idInput)

    ; Run Notepad
    Run("notepad.exe")

    ; Wait for Notepad to actually open and become active
    ; "[CLASS:Notepad]" ensures we are targeting the Notepad window specifically
    WinWaitActive("[CLASS:Notepad]")

    ; Send the keystrokes
    Send("AutoIt is typing this for you..." & "{ENTER}")
    Sleep(500) ; Wait half a second
    Send($sMessage)

    ; Show a completion message
    MsgBox($MB_OK, "Success", "Automation finished!")
EndFunc