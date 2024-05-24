import React, { ButtonHTMLAttributes, DetailedHTMLProps } from 'react'

import './style.scss'

interface IProps extends DetailedHTMLProps<ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement> {
    className?: string
    children?: React.ReactNode
}

const Button: React.FC<IProps> = ({className, children, ...others}) => {
    return (
        <button className={['default-button', className].join(' ')} {...others}>{children}</button>
    )
}

export default Button