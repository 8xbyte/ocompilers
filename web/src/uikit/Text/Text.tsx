import React, { DetailedHTMLProps, HTMLAttributes } from 'react'

import './style.scss'

interface IProps extends DetailedHTMLProps<HTMLAttributes<HTMLSpanElement>, HTMLSpanElement> {
    className?: string
    children?: React.ReactNode
}

const Text: React.FC<IProps> = ({ className, children, ...others }) => {
    return (
        <span className={['default-text', className].join(' ')} {...others}>{children}</span>
    )
}

export default Text