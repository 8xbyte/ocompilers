import React, { useEffect, useState } from 'react'
import { closeEditorWebSocket, deleteFirstEditorWebSocketMessage, openEditorWebSocket } from '../../store/slices/editorWebSocketSlice'
import { addTerminalOutputMessage, disableTerminalInput, enableClearTerminal, enableTerminalInput } from '../../store/slices/terminalSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'
import { IRuntimeMessage } from '../../types/message'

import BorderBlock from '../../uikit/BorderBlock/BorderBlock'

import RunDebugIcon from './assets/RunDebugIcon.png'
import RunIcon from './assets/RunIcon.png'

import './style.scss'
import IconButton from '../../uikit/IconButton/IconButton'
import { setEditorCurrentLanguage } from '../../store/slices/monacoEditorSlice'

const ControlBlock: React.FC = () => {
    const dispatch = useAppDispatch()

    const monacoEditor = useAppSelector(state => state.monacoEditor)
    const editorWebSocket = useAppSelector(state => state.editorWebSocket)
    const workspaceStruct = useAppSelector(state => state.workspaceStruct)

    const [socket, setSocket] = useState<WebSocket | null>(null)

    useEffect(() => {
        if (socket) {
            socket.onopen = (e) => {
                dispatch(openEditorWebSocket())
                socket.send(JSON.stringify(workspaceStruct.files.map((item) => ({
                    name: item.name,
                    content: item.content,
                    isExecutable: item.name === workspaceStruct.openned
                }))))
                dispatch(enableClearTerminal())
            }

            socket.onmessage = (e) => {
                console.log(e.data)
                try {
                    let json = JSON.parse(e.data)

                    if (json.type === 'runtime-start') {
                        dispatch(enableTerminalInput())
                    } else if (json.type === 'runtime-end') {
                        dispatch(disableTerminalInput())
                        socket.close()
                    }
                    dispatch(addTerminalOutputMessage(json.message + '\n'))
                } catch {}
            }

            socket.onerror = (e) => {
                console.log(e)
            }

            socket.onclose = (e) => {
                console.log(e.reason)
                dispatch(closeEditorWebSocket())
            }
        }

        return () => {
            socket?.close()
        }
    }, [socket])

    useEffect(() => {
        if (editorWebSocket.isOpen && editorWebSocket.messages.length > 0) {
            socket?.send(JSON.stringify(editorWebSocket.messages[0]))
            dispatch(deleteFirstEditorWebSocketMessage())
        }
    }, [editorWebSocket.messages])

    return (
        <BorderBlock className='buttons-block'>
            <div className='side-block'>
                <select onChange={(e) => {
                    dispatch(setEditorCurrentLanguage(e.target.value))
                }} className='language-select'>
                    <option className='select-option' value='cpp'>C++</option>
                    <option value='python'>Python</option>
                </select>
            </div>
            <div className='side-block'>
                <IconButton onClick={() => {
                    setSocket(new WebSocket(`wss://astra.oregona.ru/api/compilers/${monacoEditor.currentLanguage}`))
                }} className='button-icon' iconFile={RunIcon} />
                <IconButton onClick={() => {
                    setSocket(new WebSocket(`wss://astra.oregona.ru/api/compilers/${monacoEditor.currentLanguage}?isDebug=true`))
                }} className='button-icon' iconFile={RunDebugIcon} />
            </div>
        </BorderBlock>
    )
}

export default ControlBlock