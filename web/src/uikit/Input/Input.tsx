import React, { DetailedHTMLProps, InputHTMLAttributes, forwardRef } from 'react'

import './style.scss'

interface IProps extends DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>, HTMLInputElement> {
    className?: string
}

const Input: React.FC<IProps> = forwardRef(({ className, ...others }, ref) => {
    return (
        <input ref={ref} className={['default-input', className].join(' ')} {...others} />
    )
})

export default Input