import React, { useEffect, useState } from 'react'
import { setIsAuthPopupLogin, setIsShowAuthPopup } from '../../store/slices/dataSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'

import BorderBlock from '../../uikit/BorderBlock/BorderBlock'
import Button from '../../uikit/Button/Button'
import Input from '../../uikit/Input/Input'
import Text from '../../uikit/Text/Text'

import { login, register } from '../../api/auth'
import { addNotificationMessage } from '../../store/slices/notificationSlice'
import './style.scss'

interface IProps {
    isShow: boolean
}

const AuthPopup: React.FC<IProps> = (props) => {
    const dispatch = useAppDispatch()

    const data = useAppSelector(state => state.data)
    const auth = useAppSelector(state => state.auth)

    const [emailText, setEmailText] = useState<string | null>(null)
    const [passwordText, setPasswordText] = useState<string | null>(null)

    useEffect(() => {
        if (data.isAuthPopupLogin) {
            if (auth.login.status === 'success') {
                localStorage.setItem('isAuth', 'true')
                dispatch(setIsShowAuthPopup(false))
            } else if (auth.login.status === 'failed' && auth.login.error) {
                dispatch(addNotificationMessage(auth.login.error))
            }
        } else {
            if (auth.register.status === 'success') {
                localStorage.setItem('isAuth', 'false')
                dispatch(setIsShowAuthPopup(false))
            } else if (auth.login.status === 'failed' && auth.register.error) {
                dispatch(addNotificationMessage(auth.register.error))
            }
        }
    }, [auth.login, auth.register])

    const buttonHandler = () => {
        if (emailText && passwordText) {
            if (data.isAuthPopupLogin) {
                dispatch(login({
                    email: emailText,
                    password: passwordText
                }))
            } else {
                dispatch(register({
                    email: emailText,
                    password: passwordText
                }))
            }
        }
    }

    return (
        props.isShow === false ? null :
            <div onClick={(e) => {
                dispatch(setIsShowAuthPopup(false))
            }} className='auth-popup'>
                <BorderBlock onClick={(e) => {
                    e.preventDefault()
                    e.stopPropagation()
                }} className='auth-block'>
                    <Text className='auth-block-title'>{data.isAuthPopupLogin ? 'Авторизация' : 'Регистрация'}</Text>

                    <Text className='auth-input-title'>Почта</Text>
                    <Input onChange={(e) => setEmailText(e.target.value === '' ? null : e.target.value)} className='auth-input' />

                    <Text className='auth-input-title'>Пароль</Text>
                    <Input type='password' onChange={(e) => setPasswordText(e.target.value === '' ? null : e.target.value)} className='auth-input' />

                    <Button onClick={buttonHandler} className='auth-button'>{data.isAuthPopupLogin ? 'Войти' : 'Создать аккаунт'}</Button>
                    {
                        data.isAuthPopupLogin ?
                            <Text className='text-info'>Если ещё нет аккаунта, <Text onClick={() => dispatch(setIsAuthPopupLogin(false))} className='text-link'>создайте</Text></Text>
                            : <Text className='text-info'>Уже есть аккаунта, <Text onClick={() => dispatch(setIsAuthPopupLogin(true))} className='text-link'>войдите</Text></Text>
                    }
                </BorderBlock>
            </div>
    )
}

export default AuthPopup