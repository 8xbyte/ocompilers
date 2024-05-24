import React, { useEffect, useRef, useState } from 'react'

import BorderBlock from '../../uikit/BorderBlock/BorderBlock'
import Text from '../../uikit/Text/Text'

import { addNotificationMessage } from '../../store/slices/notificationSlice'
import { workspaceStructAddFile, workspaceStructDeleteFile, workspaceStructSetOpennedFile, workspaceStructWriteFile } from '../../store/slices/workspaceStructSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'
import IconButton from '../../uikit/IconButton/IconButton'
import Input from '../../uikit/Input/Input'
import './style.scss'
import { createFileWorkspaceStruct, deleteFileWorkspaceStruct, getWorkspaceStruct, writeFileWorkspaceStruct } from '../../api/workspaceStruct'

const ExplorerBlock: React.FC = () => {
    const dispatch = useAppDispatch()
    const workspace = useAppSelector(state => state.workspace)
    const workspaceStruct = useAppSelector(state => state.workspaceStruct)
    const monacoEditor = useAppSelector(state => state.monacoEditor)

    const inputRefs = useRef<(HTMLInputElement | null)[]>([])

    const [isCreate, setIsCreate] = useState<boolean>(false)

    useEffect(() => {
        dispatch(workspaceStructWriteFile(monacoEditor.text))
    }, [monacoEditor.text])

    useEffect(() => {
        if (workspace.current !== null) {
            dispatch(getWorkspaceStruct({
                id: workspace.get.result[workspace.current].id
            }))
        }
    }, [workspace.current])

    useEffect(() => {
        if (workspaceStruct.getStruct.status === 'success') {
            workspaceStruct.getStruct.result.forEach((item) => {
                dispatch(workspaceStructAddFile(item))
            })
        }
    }, [workspaceStruct.getStruct])

    return (
        <BorderBlock className='explorer-block'>
            <div className='workspace-line'>
                <Text className='workspace-title'>{workspace.current !== null ? workspace.get.result[workspace.current].name : 'Временное рабочее пространство'}</Text>
                <div className='workspace-buttons'>
                    <IconButton onClick={(e) => setIsCreate(true)} iconName='new-file' />
                </div>
            </div>
            <div className='section-line'></div>
            <div className='filesystem'>
                {workspaceStruct.files.map((item, index) =>
                    <div onClick={(e) => {
                        dispatch(workspaceStructSetOpennedFile(inputRefs.current[index]!.value))
                    }} key={index} className='file'>
                        <div className='file-block'>
                            <Input ref={element => inputRefs.current[index] = element} className='file-input' defaultValue={item.name} readOnly />
                        </div>
                        <div className='file-block'>
                            <IconButton onClick={(e) => {
                                if (workspace.current !== null && workspaceStruct.openned) {
                                    dispatch(writeFileWorkspaceStruct({
                                        workspaceId: workspace.get.result[workspace.current].id,
                                        path: inputRefs.current[index]!.value,
                                        content: monacoEditor.text
                                    }))
                                }
                            }} className='file-icon' iconName='save' />
                            <IconButton onClick={(e) => {       
                                if (workspace.current !== null) {
                                    dispatch(deleteFileWorkspaceStruct({
                                        workspaceId: workspace.get.result[workspace.current].id,
                                        path: inputRefs.current[index]!.value
                                    }))
                                }
                                dispatch(workspaceStructDeleteFile(inputRefs.current[index]!.value))
                            }} className='file-icon' iconName='trash' />
                        </div>
                    </div>)}
                {!isCreate ? null : <div className='file'>
                    <div className='file-block'>
                        <Input autoFocus onBlur={(e) => {
                            const inputString = e.target.value
                            const isFound = workspaceStruct.files.find((item) => item.name === inputString)
                            if (isFound) {
                                dispatch(addNotificationMessage("Файл с таки именем уже существует"))
                            } else {
                                if (workspace.current !== null) {
                                    dispatch(createFileWorkspaceStruct({
                                        workspaceId: workspace.get.result[workspace.current].id,
                                        path: inputString
                                    }))
                                }
                                dispatch(workspaceStructAddFile({
                                    type: 'file',
                                    name: inputString,
                                    content: ''
                                }))
                            }
                            setIsCreate(false)
                        }} className='file-input' defaultValue={'new file'} />
                    </div>
                    <div className='file-block'>
                        <IconButton className='file-icon' iconName='save' />
                        <IconButton className='file-icon' iconName='trash' />
                    </div>
                </div>}
            </div>
        </BorderBlock>
    )
}

export default ExplorerBlock