import { Uri, editor } from 'monaco-editor';
import React, { useEffect, useRef, useState } from 'react';
import { setEditorText } from '../../store/slices/monacoEditorSlice';
import { useAppDispatch, useAppSelector } from '../../store/store';

import debounce from 'debounce';

import './style.scss';

interface IProps {
    theme: 'vs' | 'vs-dark' | 'hc-black' | 'hc-light'
    value: string
    language: string
}

const Editor: React.FC<IProps> = (props) => {
    const dispatch = useAppDispatch()

    const monacoEditorRef = useRef<HTMLDivElement>(null)
    const [monacoEditor, setMonacoEditor] = useState<editor.IStandaloneCodeEditor | null>(null)

    const workspaceStruct = useAppSelector(state => state.workspaceStruct)

    const monEditor = useAppSelector(state => state.monacoEditor)

    useEffect(() => {
        let languageModel = monacoEditor?.getModel()
        if (languageModel) {
            editor.setModelLanguage(languageModel, monEditor.currentLanguage)
        }
    }, [monEditor.currentLanguage])

    useEffect(() => {
        console.log(monacoEditor, workspaceStruct.openned)
        if (monacoEditor && workspaceStruct.openned) {
            const file = workspaceStruct.files.find((item) => item.name === workspaceStruct.openned)
            if (file) {
                monacoEditor.setValue(file.content)
            }
        }
    }, [workspaceStruct.openned, monacoEditor])

    useEffect(() => {
        if (monacoEditorRef.current) {
            setMonacoEditor(editor.create(monacoEditorRef.current, {
                theme: props.theme,
                formatOnPaste: true,
                formatOnType: true,
                language: props.language,
                minimap: {
                    enabled: false
                },
                value: props.value
            }))

            dispatch(setEditorText(props.value))
        }

        return () => monacoEditor?.dispose()
    }, [])

    useEffect(() => {
        if (monacoEditor) {
            const model = monacoEditor.getModel()
            const onChange = model?.onDidChangeContent(() => {
                dispatch(setEditorText(model.getValue()))
            })

            return () => onChange?.dispose()
        }
    }, [monacoEditor])

    useEffect(() => {
        if (!monacoEditor || !monacoEditorRef.current) return

        const resizeEditor = () => {
            monacoEditor?.layout({
                width: 0, height: 0
            })

            window.requestAnimationFrame(() => {
                const rect = monacoEditorRef.current?.getBoundingClientRect()
                if (rect && rect.width && rect.height) {
                    monacoEditor?.layout({
                        width: rect.width, height: rect.height
                    })
                }
            })
        }

        const debounced = debounce(resizeEditor, 100)
        window.addEventListener('resize', debounced)
        return () => window.removeEventListener('resize', debounced)
    }, [monacoEditor, monacoEditorRef.current])

    return (
        <div className='monaco-editor-container' ref={monacoEditorRef}></div>
    )
}

export default Editor