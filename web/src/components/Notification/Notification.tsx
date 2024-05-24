import React, { useEffect } from 'react'

import { deleteFirstNotificationMessage } from '../../store/slices/notificationSlice'
import { useAppDispatch, useAppSelector } from '../../store/store'
import BorderBlock from '../../uikit/BorderBlock/BorderBlock'
import './style.scss'

interface IProps {
    timeout: number
}

const Notification: React.FC<IProps> = ({ timeout }) => {
    const dispatch = useAppDispatch()

    const notification = useAppSelector(state => state.notification)

    useEffect(() => {
        if (notification.messages.length > 0) {
            let timeoutId = setTimeout(() => {
                dispatch(deleteFirstNotificationMessage())
            }, timeout)
            return () => clearTimeout(timeoutId)
        }
    }, [notification.messages])


    return notification.messages.length > 0 ? (
        <div className='notification-block'>
            <BorderBlock className='notification'>
                {notification.messages[0]}
            </BorderBlock>
        </div>
    ) : null
}

export default Notification