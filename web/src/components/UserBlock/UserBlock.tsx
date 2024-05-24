import { setIsShowAuthPopup } from '../../store/slices/dataSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'

import React, { useEffect, useRef, useState } from 'react'
import BorderBlock from '../../uikit/BorderBlock/BorderBlock'
import Button from '../../uikit/Button/Button'
import Text from '../../uikit/Text/Text'

import { createWorkspace, deleteWorkspace, getWorkspaces, renameWorkspace } from '../../api/workspace'
import { setIsAuth } from '../../store/slices/authSlice'
import { addNotificationMessage } from '../../store/slices/notificationSlice'
import IconButton from '../../uikit/IconButton/IconButton'
import Input from '../../uikit/Input/Input'
import './style.scss'
import { workspaceSetCurrent } from '../../store/slices/workspaceSlice'
import { workspaceClearFiles } from '../../store/slices/workspaceStructSlice'

const UserBlock: React.FC = () => {
    const dispatch = useAppDispatch()

    const inputRef = useRef<(HTMLInputElement | null)[]>([])

    const auth = useAppSelector(state => state.auth)
    const workspace = useAppSelector(state => state.workspace)

    const [isCreate, setIsCreate] = useState(false)

    useEffect(() => {
        dispatch(getWorkspaces())
    }, [])

    if (auth.isAuth) {
        return (
            <BorderBlock className='user-block'>
                {isCreate ?
                    <BorderBlock className='workspace-block'>
                        <Input onBlur={(e) => {
                            setIsCreate(false)
                            const workspaceName = e.target.value
                            const isExists = workspace.get.result.some(item => item.name === workspaceName)

                            if (isExists) {
                                dispatch(addNotificationMessage("Рабочее пространство с таким именем уже существует"))
                            } else if (workspaceName.length < 3) {
                                dispatch(addNotificationMessage("Минимальный размер имени рабочего простарнства 3 символа"))
                            } else {
                                dispatch(createWorkspace({
                                    name: workspaceName
                                }))
                            }
                        }} className='workspace-input' />
                        <div className='workspace-buttons'>
                            <IconButton className='workspace-button' iconName='edit' />
                            <IconButton className='workspace-button' iconName='trash' />
                        </div>
                    </BorderBlock> : null}
                {workspace.get.result.map((item, index) =>
                    <BorderBlock onClick={(e) => {
                        dispatch(workspaceClearFiles())
                        dispatch(workspaceSetCurrent(index))
                    }} className='workspace-block' key={index}>
                        <Input ref={element => inputRef.current[index] = element} defaultValue={item.name} readOnly onBlur={(e) => {
                            const workspaceName = e.target.value

                            if (workspaceName.length < 3) {
                                dispatch(addNotificationMessage("Минимальный размер имени рабочего простарнства 3 символа"))
                                e.target.value = item.name
                            } else if (workspaceName === item.name) {
                                e.target.value = item.name
                            } else {
                                dispatch(renameWorkspace({
                                    id: item.id,
                                    newName: e.target.value
                                }))
                            }

                            inputRef.current[index]!.readOnly = true
                        }} className='workspace-input' />
                        <div className='workspace-buttons'>
                            <IconButton onClick={(e) => {
                                inputRef.current[index]!.readOnly = false
                                inputRef.current[index]!.focus()
                            }} className='workspace-button' iconName='edit' />
                            <IconButton onClick={(e) => {
                                dispatch(deleteWorkspace({
                                    id: item.id
                                }))
                            }} className='workspace-button' iconName='trash' />
                        </div>
                    </BorderBlock>)}
                <Button onClick={() => {
                    if (workspace.get.result.length === 5) {
                        dispatch(addNotificationMessage("Нельзя создавать больше 5 рабочих областей"))
                    } else {
                        setIsCreate(true)
                    }
                }}>Создать рабочее пространство</Button>
                <Button onClick={() => dispatch(setIsAuth(false))}>Выйти</Button>
            </BorderBlock>
        )
    } else {
        return (
            <BorderBlock className='user-block'>
                <Text>Для того что-бы использовать рабочее пространство необходимо авторизоваться</Text>
                <Button onClick={() => dispatch(setIsShowAuthPopup(true))}>Войти</Button>
            </BorderBlock>
        )
    }
}

export default UserBlock