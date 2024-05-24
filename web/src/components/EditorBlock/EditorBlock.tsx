import React from 'react'

import ControlBlock from '../ControlBlock/ControlBlock'
import Editor from '../Editor/Editor'
import Terminal from '../Terminal/Terminal'

import './style.scss'
import { useAppSelector } from '../../store/store'

const EditorBlock: React.FC = () => {
    const monacoEditor = useAppSelector(state => state.monacoEditor)

    return (
        <div className='editor-block'>
            <ControlBlock />
            <Editor language={monacoEditor.currentLanguage} theme='vs-dark' value='' />
            <Terminal />
        </div>
    )
}

export default EditorBlock