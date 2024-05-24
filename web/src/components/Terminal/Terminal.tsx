import React, { useEffect, useRef } from 'react'
import { addEditorWebSocketMessage } from '../../store/slices/editorWebSocketSlice'
import { addTerminalInputText, clearTerminalInputText, deleteFirstTerminalOutputMessage, deleteLastTerminalInputSymbol, disableClearTerminal } from '../../store/slices/terminalSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'
import { IRuntimeMessage } from '../../types/message'

import './style.scss'

const Terminal: React.FC = () => {
    const dispatch = useAppDispatch()

    const editorWebSocket = useAppSelector(state => state.editorWebSocket)
    const terminal = useAppSelector(state => state.terminal)

    const textareaRef = useRef<HTMLTextAreaElement>(null)

    useEffect(() => {
        if (textareaRef.current) {
            textareaRef.current.readOnly = !terminal.isInput
        }
    }, [terminal.isInput])

    useEffect(() => {
        if (terminal.isClearTerminal && textareaRef.current) {
            textareaRef.current.value = ''
            dispatch(disableClearTerminal())
        }
    }, [terminal.isClearTerminal])

    useEffect(() => {
        if (textareaRef.current && terminal.outputTextBuffer.length > 0) {
            textareaRef.current.value += terminal.outputTextBuffer[0]
            dispatch(deleteFirstTerminalOutputMessage())
        }
    }, [terminal.outputTextBuffer])

    return (
        <div className='terminal-block'>
            <textarea readOnly ref={textareaRef} className='terminal-area' onKeyDown={(e) => {
                if (!textareaRef.current?.readOnly) {
                    if (e.key === 'Backspace') {
                        if (terminal.inputText.length > 0) {
                            dispatch(deleteLastTerminalInputSymbol())
                        }
                    } else if (e.key === 'Enter') {
                        if (editorWebSocket.isOpen) {
                            dispatch(addEditorWebSocketMessage({
                                type: 'runtime-stdin',
                                message: terminal.inputText + '\n'
                            } as IRuntimeMessage))

                            dispatch(clearTerminalInputText())
                        }
                    } else if (e.key.length === 1) {
                        dispatch(addTerminalInputText(e.key))
                    }
                }
            }} />
        </div>
    )
}

export default Terminal